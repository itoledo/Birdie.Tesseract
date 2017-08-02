namespace tvn.cosine.ai.libsvm
{
    public interface svm_print_interface
    {
        void print(string message);
    }

    public class Defaultsvm_print_interface : svm_print_interface
    {
        public void print(string message)
        {
            System.Console.WriteLine(message);
        }
    }

    public class NONEsvm_print_interface : svm_print_interface
    {
        public void print(string message)
        { }
    }
}
