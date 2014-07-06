using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity.Publishers
{
    public class Publisher<T> : IObservable<T>
    {
        private List<IObserver<T>> observers;

        public Publisher()
        {
            observers = new List<IObserver<T>>();
        }
        /// <summary>
        /// Notify is not part of the IObserver interface - but the Publisher needs it to notify all registered subscribers 
        /// </summary>
        /// <param name="obj"></param>
        protected void Notify(T obj)
        {
            foreach (IObserver<T> observer in observers)
            {
                observer.OnNext(obj);
            }
        }
        /// <summary>
        /// This is the only method that the IObservver interface contains - and thus, needs to be implemented
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new RemoveMe(observers, observer);
        }

        private class RemoveMe : IDisposable
        {
            private List<IObserver<T>> observers;
            private IObserver<T> observer;

            public RemoveMe(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observer != null && observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
        }
    }
}
