using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using PorEjemplo.Helpers;
using System.Reflection;
using System.Collections.Generic;

namespace PorEjemplo {

    class Program {

        static void Main(string[] args) {

            var gen = Generator<Test>.Setup()
                .ForMember(_ => _.Nothing).Ignore()
                .ForType<string>().UseGenerator(new HelloGenerator())
                .GetGenerator();

            var obj = (Test) gen.Generate();
        }
    }

    public interface IGenerator {

        object Generate();

    }

    public class HelloGenerator : IGenerator {

        public object Generate() {
            return "Hello";
        }
    }

    public class Generator<TSource> : IGenerator where TSource : new() {
        
        private Dictionary<string, object> Values { get; }
        private Dictionary<string, IGenerator> Generators { get; }
        private List<string> Ignore { get; }

        private Dictionary<Type, IGenerator> TypeGenerators { get; }

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

                if (TypeGenerators.ContainsKey(prop.PropertyType)) {
                    try {
                        prop.SetValue(obj, TypeGenerators[prop.PropertyType].Generate(), null);
                    } catch (ArgumentException) {
                        throw new ArgumentException($"Mismatching generator type for type {prop.PropertyType}");
                    }
                continue;
                }
                
                if(prop.PropertyType == typeof(string)) {
                    prop.SetValue(obj, ExtendedRandom.NextString(10), null);
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
            
            public TypeSetup ForType<T>() {
                return new TypeSetup(this, typeof(T));
            }

            public Generator<TSource> GetGenerator() {
                return _generator;
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

            public class TypeSetup {

                private readonly GeneratorSetup _setup;
                private readonly Type _type;

                internal TypeSetup(GeneratorSetup setup, Type type) {
                    _setup = setup;
                    _type = type;
                }

                public GeneratorSetup UseGenerator(IGenerator generator) {
                    _setup._generator.TypeGenerators.Add(_type, generator);
                    return _setup;
                }

            }

        }
    }

    

    public class Test {
        
        public string Text { get; set; }

        public string Jo { get; set; }

        public string Nothing { get; set; }

    }
}