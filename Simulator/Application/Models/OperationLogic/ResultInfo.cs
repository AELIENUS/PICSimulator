using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Application.Services
{
    public class ResultInfo
    {
        public int? JumpAddress;
        public bool ClearISR;
        /// <summary>
        /// Is 1, when C needs to be set. Is 0, when C needs to be cleared. Null if not affected
        /// </summary>
        public OverflowInfo OverflowInfo;
        public bool CheckZ;
        public int? PCIncrement;
        public int? Cycles;
        //List of tuple containing result first then address; This is a list because a return operation writes to memory two times
        public List<OperationResult> OperationResults;
        public bool BeginLoop;

    }
}