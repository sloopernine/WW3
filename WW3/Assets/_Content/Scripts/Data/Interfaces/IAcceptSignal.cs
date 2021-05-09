using Data.Enums;

namespace Data.Interfaces
{
    public interface IAcceptSignal
    {
        public void ReceiveSignal(Signal signal);
    }
}