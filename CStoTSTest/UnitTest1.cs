using CStoTS;
using CStoTS.ApplicationService;
using CStoTSTest.Common;
using Xunit;

namespace CStoTSTest
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      // ��U����

      // C#�\�[�X�쐬
      var testBase = new TestBase();
      testBase.CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }", null);

      // �ϊ��E�o��
      var csToTs = new ConvertApplication();
      csToTs.Convert(string.Empty, string.Empty, new TSFileRepositoryMock(), testBase.Files);
    }
  }
}
