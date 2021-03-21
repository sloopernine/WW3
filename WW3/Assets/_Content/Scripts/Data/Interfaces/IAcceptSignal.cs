using Data.DataContainers;

namespace Data.Interfaces
{
    public interface IAcceptSignal
    {
        public void ReceiveSignal(Signal signal);
    }
}