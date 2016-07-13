using System;
using System.Collections;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public class CustomIterator : IEnumerator<int>
    {
        //private int _firstNum=-1;
        private int _firstNum = 0;
        private int _secondNum = 1;
        private int _current;

        public int Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            try
            {
                checked
                {
                    _current = _firstNum + _secondNum;
                }
                _firstNum = _secondNum;
                _secondNum = _current;
                return true;
            }
            catch (OverflowException exp)
            {
                throw new OverflowException("Overflow exception in iterator", exp);
            }
        }

        public void Reset()
        {
            //_firstNum = -1;
            _firstNum = 0;
            _secondNum = 1;
        }
    }
}
