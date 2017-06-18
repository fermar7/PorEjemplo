using System;
using System.Linq.Expressions;
using PorEjemplo.Helpers;
using System.Reflection;
using System.Collections.Generic;

namespace PorEjemplo.Generator {

    public sealed class Generator<TSource> : IGenerator where TSource : new() {
        
        private Dictionary<string, object> Values { get; }
        private Dictionary<string, IGenerator> Generators { get; }
        private List<string> Ignore { get; }
        private Dictionary<Type, IGenerator> TypeGenerators { get; }
        private bool OnlyCustom { get; set; } = false;

        private static Random Random { get; } = new Random();

        public Generator() {
            Values = new Dictionary<string, object>();
            Generators = new Dictionary<string, IGenerator>();
            Ignore = new List<string>();
            TypeGenerators = new Dictionary<Type, IGenerator>();
        }

        public object Generate() {
            var obj = new TSource();
            foreach (var prop in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if(Ignore.Contains(prop.Name)) {
                    continue;
                }
                
                if(Values.ContainsKey(prop.Name)) {
                    prop.SetValue(obj, Values[prop.Name], null);
                    continue;
                }

                if (Generators.ContainsKey(prop.Name)) {
                    try {
                        prop.SetValue(obj, Generators[prop.Name].Generate(), null);
                    } catch(ArgumentException) {
                        throw new ArgumentException($"Mismatching generator type for property {prop.Name}");
                    }
                    continue;
                }

                if (OnlyCustom) {
                    continue;
                }
                
                if (TypeGenerators.ContainsKey(prop.PropertyType)) {
                    try {
                        prop.SetValue(obj, TypeGenerators[prop.PropertyType].Generate(), null);
                    } catch (ArgumentException) {
                        throw new ArgumentException($"Mismatching generator type for type {prop.PropertyType}");
                    }
                    continue;
                }

                Type type = prop.PropertyType;

                if (type == typeof(string)) {
                    prop.SetValue(obj, ExtendedRandom.NextString(10), null);
                } else if (type == typeof(int) || type == typeof(long) || type == typeof(uint) || type == typeof(ulong)) {
                    prop.SetValue(obj, Random.Next(1, 500), null);
                } else if (type == typeof(byte)) {
                    prop.SetValue(obj, Random.Next(0, 256), null);
                } else if (type == typeof(sbyte)) {
                    prop.SetValue(obj, Random.Next(-128, 128), null);
                } else if (type == typeof(short) || type == typeof(ushort)) {
                    prop.SetValue(obj, Random.Next(0, 32768), null);
                } else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) {
                    prop.SetValue(obj, Convert.ChangeType(ExtendedRandom.NextDouble(0, 500), type), null);
                } else if (type == typeof(bool)) {
                    prop.SetValue(obj, ExtendedRandom.NextBoolean(), null);
                } else if (type == typeof(char)) {
                    prop.SetValue(obj, ExtendedRandom.NextChar(), null);
                } else if (type == typeof(Guid)) {
                    prop.SetValue(obj, Guid.NewGuid(), null);
                } else if (type == typeof(DateTime)) {
                    prop.SetValue(obj, ExtendedRandom.NextDateTime(), null);
                }
                

            }
            return obj;
        }

        public static GeneratorSetup Setup() {
            return new GeneratorSetup();
        }

        public Type GetSourceType() {
            return typeof(TSource);
        }

        public class GeneratorSetup {
            private readonly Generator<TSource> _generator;

            internal GeneratorSetup() {
                _generator = new Generator<TSource>();
            }

            public MemberSetup<TMember> ForMember<TMember>(Expression<Func<TSource, TMember>> selector) {
                if (selector.Body is MemberExpression expression) {
                    return new MemberSetup<TMember>(this, expression.Member.Name);
                } else {
                    throw new ArgumentException("", nameof(selector));
                }
            }
            
            public TypeSetup<T> ForType<T>() {
                return new TypeSetup<T>(this);
            }

            public Generator<TSource> GetGenerator() {
                return _generator;
            }

            public GeneratorSetup UseOnlySpecifiedMembers() {
                _generator.OnlyCustom = true;
                return this;
            }

            public class MemberSetup<TMember> {

                private readonly GeneratorSetup _setup;
                private readonly string _name;

                internal MemberSetup(GeneratorSetup setup, string name) {
                    _setup = setup;
                    _name = name;
                }

                public GeneratorSetup UseValue(TMember value) {
                    _setup._generator.Values.Add(_name, value);
                    return _setup;
                }

                public GeneratorSetup UseGenerator(IGenerator generator) {
                    _setup._generator.Generators.Add(_name, generator);
                    return _setup;
                }

                public GeneratorSetup Ignore() {
                    _setup._generator.Ignore.Add(_name);
                    return _setup;
                }

            }

            public class TypeSetup<T> {

                private readonly GeneratorSetup _setup;

                internal TypeSetup(GeneratorSetup setup) {
                    _setup = setup;
                }

                public GeneratorSetup UseGenerator(IGenerator generator) {
                    _setup._generator.TypeGenerators.Add(typeof(T), generator);
                    return _setup;
                }

            }

        }
    }
}