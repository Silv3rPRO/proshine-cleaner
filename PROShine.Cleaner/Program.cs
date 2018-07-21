using Mono.Cecil;

namespace PROShine.Cleaner
{
    internal class Program
    {
        internal static void Main()
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("Assembly-CSharp-original.dll");

            new UnusedMethodsRemover(assembly).Execute();
            new ElementsRenamer(assembly).Execute();

            assembly.Write("Assembly-CSharp.dll");
        }
    }
}
