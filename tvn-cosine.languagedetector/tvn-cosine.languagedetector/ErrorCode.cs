using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn_cosine.languagedetector
{
    public enum ErrorCode
    {
        NoTextError, FormatError, FileLoadError, DuplicateLangError, NeedLoadProfileError, CantDetectError, CantOpenTrainData, TrainDataFormatError, InitParamError
    }
}
