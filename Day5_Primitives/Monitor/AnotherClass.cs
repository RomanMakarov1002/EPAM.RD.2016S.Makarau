using System.Threading;


namespace Monitor
{
    // TODO: Use SpinLock to protect this structure.
    public class AnotherClass
    {
        private int _value;
        private SpinLock _sl = new SpinLock();
        

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
            bool lockTaken = false;
            try
            {
                _sl.Enter(ref lockTaken);
                _value++;
            }
            finally
            {
                if (lockTaken)
                    _sl.Exit();
            }
            
        }

        public void Decrease()
        {
            bool lockTaken = false;
            try
            {
                _sl.Enter(ref lockTaken);
                _value--;
            }
            finally
            {
                if (lockTaken)
                    _sl.Exit();
            }
        }
    }
}
