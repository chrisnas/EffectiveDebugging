using System;
using System.Diagnostics;

namespace Memory
{
    class DisposableWrapper : IDisposable
    {
        private bool _disposed;
        private IDisposable _managedResource;
        private int _nativeResource;


        public DisposableWrapper(int nativeResource)
        {
            _nativeResource = nativeResource;
            _disposed = false;
        }

        // Finalizer
        ~DisposableWrapper()
        {
            WriteLine("~DisposableWrapper()");
            Cleanup("called from GC" != null);
        }                       //    ^-- true 


        // IDisposable implementation
        public void Dispose()
        {
            WriteLine("Dispose()");
            Cleanup("not in GC" == null);
        }                   //   ^-- false 


        // Note: since this class is not sealed, make it protected virtual
        //       in sealed case, make it private
        protected virtual void Cleanup(bool fromGC)
        {
            try
            {
                // allow multiple calls to IDisposable.Dispose()
                // but only the first one does something
                if (_disposed)
                {
                    WriteLine("Already disposed");
                    return;
                }

                // always clean up native resources
                // _managedResource is usually not an int   :^)
                // --> free _managedResource here and set it to null
                WriteLine("Cleanup native resources");
                _nativeResource = -1 * _nativeResource;
                // TIP: a negative value in the traces means
                //      that the object has been finalized
                //      because it cannot be disposed more than once

                // no need to cleanup managed objects if in GC
                if (fromGC)
                    return;

                // clean up managed objects
                WriteLine("Cleanup managed resources");
                if (_managedResource != null)
                {
                    _managedResource.Dispose();
                    _managedResource = null;
                }
            }
            finally
            {
                // cleanup has been done and the object should not be used any more
                // even though it is possible to call Dispose but without further effect
                _disposed = true;

                // in case of child class
                // --> don't forget to call base.Cleanup(fromGC);

                // remove it from the finalizer list
                if (!fromGC)
                    GC.SuppressFinalize(this);
                // otherwise...
                // [-] ~DisposableWrapper()
                // [-] Already disposed
            }
        }

        private int _value;

        public int Value
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DisposableWrapper));

                return _value;
            }
            set { _value = value; }
        }



        private void WriteLine(string line)
        {
            Trace.WriteLine(string.Format($"[{_nativeResource}] {line}"));
        }
    }

}
