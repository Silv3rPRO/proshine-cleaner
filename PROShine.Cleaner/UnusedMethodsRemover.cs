using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PROShine.Cleaner
{
    public class UnusedMethodsRemover
    {
        private static readonly Regex ObfuscatedName = new Regex("^[A-Z0-9]{11}$");

        private readonly AssemblyDefinition assembly;
        private readonly HashSet<string> calledMethods = new HashSet<string>();

        public UnusedMethodsRemover(AssemblyDefinition assembly)
        {
            this.assembly = assembly;
        }

        public void Execute()
        {
            const int passCount = 5;

            for (int i = 0; i < passCount; ++i)
            {
                ExecuteSinglePass();
            }
        }

        private void ExecuteSinglePass()
        {
            calledMethods.Clear();

            foreach (ModuleDefinition module in assembly.Modules)
            {
                InitCalledMethods(module);
            }

            foreach (ModuleDefinition module in assembly.Modules)
            {
                foreach (TypeDefinition type in module.Types)
                {
                    RemoveUnusedMethods(type);
                }
            }
        }

        private bool IsMethodCalled(MemberReference targetMethod)
        {
            return calledMethods.Contains(targetMethod.FullName);
        }

        private void InitCalledMethods(ModuleDefinition module)
        {
            foreach (TypeDefinition type in module.Types)
            {
                InitCalledMethods(type);
            }
        }

        private void InitCalledMethods(TypeDefinition type)
        {
            foreach (TypeDefinition nestedType in type.NestedTypes)
            {
                InitCalledMethods(nestedType);
            }
            foreach (MethodDefinition method in type.Methods)
            {
                if (!method.HasBody) continue;

                foreach (Instruction instruction in method.Body.Instructions)
                {
                    if (instruction.Operand is MethodReference calledMethod)
                    {
                        calledMethods.Add(calledMethod.FullName);
                    }
                }
            }
        }

        private void RemoveUnusedMethods(TypeDefinition type)
        {
            foreach (TypeDefinition nestedType in type.NestedTypes)
            {
                RemoveUnusedMethods(nestedType);
            }

            if (!ObfuscatedName.IsMatch(type.Name)) return;

            if (type.IsAbstract || type.IsInterface || type.GenericParameters.Count > 0) return;

            foreach (MethodDefinition method in type.Methods.ToArray())
            {
                if (method.GenericParameters.Count > 0 || method.IsSetter || method.IsGetter ||
                    method.IsVirtual || method.IsAbstract || method.HasOverrides ||
                    method.IsCompilerControlled || method.IsConstructor || method.IsPInvokeImpl ||
                    method.IsUnmanaged || method.IsUnmanagedExport) continue;

                if (!ObfuscatedName.IsMatch(method.Name)) continue;

                if (IsMethodCalled(method)) continue;

                Console.WriteLine(method.Name + " is unused, removing");
                type.Methods.Remove(method);
            }
        }
    }
}
