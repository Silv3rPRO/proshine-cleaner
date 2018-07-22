using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PROShine.Cleaner
{
    public class ElementsRenamer
    {
        public Dictionary<string, string> RenamedClasses { get; } = new Dictionary<string, string>();

        private static readonly Regex ObfuscatedName = new Regex("^[A-Z0-9]{11}$");

        private readonly AssemblyDefinition assembly;

        private int classCount;
        private int propertyCount;
        private int fieldCount;
        private int methodCount;
        private int paramCount;

        public ElementsRenamer(AssemblyDefinition assembly)
        {
            this.assembly = assembly;
        }

        public void Execute()
        {
            foreach (ModuleDefinition module in assembly.Modules)
            {
                RenameElements(module);
            }
        }

        private void RenameElements(ModuleDefinition module)
        {
            foreach (TypeDefinition type in module.Types)
            {
                RenameElements(type);
            }
        }

        private void RenameElements(TypeDefinition type)
        {
            foreach (TypeDefinition nestedType in type.NestedTypes)
            {
                RenameElements(nestedType);
            }

            if (!ObfuscatedName.IsMatch(type.Name)) return;
            if (type.IsAbstract || type.IsInterface || type.GenericParameters.Count > 0 || !type.IsClass || type.IsEnum) return;

            classCount += 1;
            string newClassName = "Class" + classCount;

            Console.WriteLine("Renaming class " + type.Name + " to " + newClassName);
            RenamedClasses.Add(type.Name, newClassName);
            type.Name = newClassName;

            foreach (FieldDefinition field in type.Fields)
            {
                RenameField(field);
            }

            foreach (PropertyDefinition property in type.Properties)
            {
                RenameProperty(property);
            }

            foreach (MethodDefinition method in type.Methods)
            {
                RenameMethod(method);

                if (!method.HasParameters) continue;

                paramCount = 0;
                foreach (ParameterDefinition parameter in method.Parameters)
                {
                    RenameParameter(parameter);
                }
            }
        }

        private void RenameMethod(MethodDefinition method)
        {
            if (!ObfuscatedName.IsMatch(method.Name)) return;

            if (method.IsVirtual || method.IsSetter || method.IsGetter || method.IsVirtual || method.IsAbstract ||
                method.HasOverrides || method.IsCompilerControlled || method.IsConstructor || method.IsPInvokeImpl ||
                method.IsUnmanaged || method.IsUnmanagedExport) return;

            methodCount += 1;
            string newMethodName = "Method" + methodCount;

            Console.WriteLine("Renaming method " + method.Name + " to " + newMethodName);
            method.Name = newMethodName;
        }

        private void RenameField(FieldDefinition field)
        {
            if (!ObfuscatedName.IsMatch(field.Name)) return;

            fieldCount += 1;
            string newFieldName = "field" + fieldCount;

            Console.WriteLine("Renaming field " + field.Name + " to " + newFieldName);
            field.Name = newFieldName;
        }

        private void RenameProperty(PropertyDefinition field)
        {
            if (!ObfuscatedName.IsMatch(field.Name)) return;

            propertyCount += 1;
            string newFieldName = "Property" + propertyCount;

            Console.WriteLine("Renaming property " + field.Name + " to " + newFieldName);
            field.Name = newFieldName;
        }

        private void RenameParameter(ParameterReference parameter)
        {
            if (!ObfuscatedName.IsMatch(parameter.Name)) return;

            paramCount += 1;
            string newParamName = "param" + paramCount;

            Console.WriteLine("Renaming parameter " + parameter.Name + " to " + newParamName);
            parameter.Name = newParamName;
        }
    }
}
