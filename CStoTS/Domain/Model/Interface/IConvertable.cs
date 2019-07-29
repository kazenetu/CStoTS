using CSharpAnalyze.Domain.PublicInterfaces;

namespace CStoTS.Domain.Model.Interface
{
  /// <summary>
  /// 変換インターフェース
  /// </summary>
  internal interface IConvertable
  {
    string Convert(IAnalyzeItem item, int indent);
  }
}
