' ***^^^***|||***^^^***' ' ' ' %%%###%%%806e1b96fb026b84fd7e28a0c231abc0%%%###%%%' ***^^^***|||***^^^***//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace CodeDOMTest {
    using Fred2 = System;
    
    
    public class Startup {
        
        public static void Main() {
            int iSum;
            int iValue = 42;
            System.IO.Stream stream;
            string fileName = "Test.txt";
            System.Console.WriteLine("Hello World");
            iSum = (42 + 23);
            stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            int[] aInts;
            int[] a2Ints;
            a2Ints = new int[10];
            aInts = new int[] {
                    0,
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9};
            iValue = aInts[3];
            int i = 0;
            aInts[i] = aInts[(i + 1)];
            if ((i > 6)) {
                System.Console.WriteLine("True Executed");
            }
            else {
                System.Console.WriteLine("False Executed");
            }
            for (i = 0; (i <= 9); i = (i + 1)) {
                System.Console.WriteLine(aInts[i]);
            }
            obj.MethodA(obj.MethodC(j));
            Startup.EqualityDifference();
        }
        
        static void EqualityDifference() {
            int iValue;
            if ((iValue 
                        = (42 + 23))) {
                System.Console.WriteLine("True Executed");
            }
            else {
                System.Console.WriteLine("False Executed");
            }
        }
    }
    
    public class TestClass : TestBase, ITest, ITest2, ITest3 {
        
        public virtual void Foo() {
        }
        
        private void DefaultScope() {
            // 5002-20482
        }
        
        void ITest3.BarZ() {
            // 5000-Private
        }
        
        private void ScopeTest() {
            // 5002-20482
        }
        
        public new virtual void TestShadows() {
            // 6010-24592
        }
        
        public override void TestOverride() {
            // 6004-24580
        }
        
        public virtual void TestOverloaded2() {
            // 6100-24832
        }
        
        void TestFinal() {
            // 2-Final
        }
        
        void TestFinalAndOverrides() {
            // 6-6
        }
    }
    
    public class TestClass : TestBase, ITest, ITest2, ITest3 {
        
        public virtual void Foo() {
        }
        
        private void DefaultScope() {
            // 5002-20482
        }
        
        void ITest3.BarZ() {
            // 5000-Private
        }
        
        private void ScopeTest() {
            // 5002-20482
        }
        
        public new virtual void TestShadows() {
            // 6010-24592
        }
        
        public override void TestOverride() {
            // 6004-24580
        }
        
        public virtual void TestOverloaded2() {
            // 6100-24832
        }
        
        void TestFinal() {
            // 2-Final
        }
        
        void TestFinalAndOverrides() {
            // 6-6
        }
    }
    
    public class TestBase {
        
        public virtual void Test() {
        }
        
        void Test2() {
        }
    }
    
    abstract class TestMustInherit {
        
        private void Test() {
        }
    }
    
    sealed class TestNotInheritable {
        
        public virtual void Test() {
        }
    }
    
    class TestNotPublic {
        
        public virtual void Test() {
        }
    }
    
    class TestNested {
        
        public virtual void Test() {
        }
    }
    
    public interface ITest {
        
        void Foo();
    }
    
    public interface ITest2 {
        
        void Foo();
    }
    
    public class  {
        
        class Publc {
        }
        
        public class Frend {
        }
        
        class Privte1 {
        }
        
        class Privte2 {
        }
        
        class Privte3 {
        }
        
        class Privte4 {
        }
        
        private class Privte5 {
        }
        
        public class Privte6 {
        }
    }
}
