using System;
using Cysharp.Threading.Tasks;

namespace TimeClock
{
    public interface ITimeService
    {
        UniTask<DateTime> GetTimeFromServerAsync();
    }
}