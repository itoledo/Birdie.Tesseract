namespace tvn.cosine.ai.learning.learners.svm
{
    public interface svm_print_interface
    {
        void print(string s);
    }

    public class svm_print_interface_console : svm_print_interface
    {
        public void print(string s)
        {
            System.Console.WriteLine(s);
        }
    }


    public class svm_print_interface_none : svm_print_interface
    {
        public void print(string s)
        { }
    }
}
