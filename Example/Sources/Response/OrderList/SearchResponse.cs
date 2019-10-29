using DataTransferObjects.CustomTables;
using Framework.DataTransferObject.BaseClasses;
using System;
using System.Collections.Generic;

namespace DataTransferObjects.Response.OrderList
{
  public class SearchResponse : ResponseBase<SearchResponse.SearchResponseParam>
  {
    public SearchResponse()
    {
    }

    public SearchResponse(Results result, string errorMessage) : base(result, errorMessage)
    {
    }

    public SearchResponse(Results result, string errorMessage, SearchResponseParam responseData) : base(result, errorMessage, responseData)
    {
    }

    public class SearchResponseParam
    {
      /// <summary>
      /// 検索結果
      /// </summary>
      public List<CustomTOrder> Results { get; } = new List<CustomTOrder>();

      // TypeScript変換確認用
      public DateTime dt{set;get;}
      public Dictionary<int,int> test{set;get;}
      public Dictionary<int,List<string>> test2{set;get;}

      public int? a{set;get;}
      public CustomTOrder b{set;get;}
      public int c{set;get;}=1;
      public float c1{set;get;}
      public double c2{set;get;}
      public decimal c3{set;get;}
      public bool bl{set;get;}

      public string s{set;get;}="123";
      public List<int> li{set;get;}=new List<int>();

      public bool? nb{set;get;} = true;
      public List<int?> nli{set;get;}=new List<int?>();
    }
  }
}
