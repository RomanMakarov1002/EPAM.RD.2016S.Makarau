using System.Threading;

namespace Monitor
{
    // TODO: Use Monitor (not lock) to protect this structure.
    public class MyClass
    {
        private int _value;
        private object o = new int[] { 1};

        public int Counter
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public void Increase()
        {
            try
            {
                System.Threading.Monitor.Enter(o);
                _value++;
            }
            finally
            {
                System.Threading.Monitor.Exit(o);
            }
            
        }

        public void Decrease()
        {
            try
            {
                System.Threading.Monitor.Enter(o);
                _value--;
            }
            finally
            {
                System.Threading.Monitor.Exit(o);
            }
        }
    }
}
