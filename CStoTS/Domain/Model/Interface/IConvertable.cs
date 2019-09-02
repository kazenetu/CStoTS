using CSharpAnalyze.Domain.PublicInterfaces;
using System.Collections.Generic;

namespace CStoTS.Domain.Model.Interface
{
  /// <summary>
  /// 変換インターフェース
  /// </summary>
  internal interface IConvertable
  {
    string Convert(IAnalyzeItem item, int indent, List<string> otherScripts);
  }
}
