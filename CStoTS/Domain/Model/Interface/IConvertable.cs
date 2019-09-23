using CSharpAnalyze.Domain.PublicInterfaces;
using CStoTS.Domain.Model.Mode;
using System.Collections.Generic;

namespace CStoTS.Domain.Model.Interface
{
  /// <summary>
  /// 変換インターフェース
  /// </summary>
  internal interface IConvertable
  {
    string Convert(IAnalyzeItem item, Config config, int indent, List<string> otherScripts);
  }
}
