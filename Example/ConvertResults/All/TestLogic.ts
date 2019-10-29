import { Dictionary } from './Dictionary';
import { KeyCollection } from './KeyCollection';
import { List } from './List';

/**
 * テストクラス
 */
export class TestLogic {
  /**
   * 定数フィールド
   */
  public ConstField: string = "123";
  private _prop_: string = "";
  /**
   * プロパティ例
   */
  public set prop(value: string) {
    this._prop_ = value;
  }
  /**
   * プロパティ例
   */
  public get prop(): string {
    return this._prop_;
  }
  private _propInt_: number = 50;
  public set propInt(value: number) {
    this._propInt_ = value;
  }
  public get propInt(): number {
    return this._propInt_;
  }
  /**
   * メソッド例
   */
  public Method(): void {
    let a = (arg: number): number => {
      let b: number = arg;
      b += arg;
      return b;
    };
    let test: number = a(10);
    if (this.prop.length > 0) {
      test = this.Method2(this.prop);
    }
    // while構文(インクリメント)
    let index: number = 0;
    while (index < 9) {
      this.propInt = index;
      // 条件用変数をインクリメント
      index++;
    }
    // while構文(デクリメント)
    index = 9;
    while (index > 0) {
      test = index;
      // 条件用変数をデクリメント
      index--;
    }
    let dc: Dictionary<string, TestLogic> = new Dictionary<string, TestLogic>();
    dc.Add("aa", this);
    let value: TestLogic = dc["aa"];
    for (let [k, v] of dc.Keys) {
      let key = { "Key": k, "Value": v };
      test = dc[key].propInt;
      let strTest: string = dc[key].toString();
    }
    this.prop = dc.Keys.toString();
    dc.Clear();
    let lst: List<string> = new List<string>();
    lst.Add("aaa");
    for (let item of lst) {
      this.prop = item;
    }
    lst.Remove("aaa");
    lst.Clear();
    this.prop = '';
    this.prop = TestLogic.ConstField;
  }
  /**
   * メソッド例2 A
   * @param src 文字列
   * @returns 戻り値
   */
  private Method21(src: string): number {
    return Number(src);
  }
  /**
   * メソッド例2 B
   * @param src 数値
   * @returns 戻り値
   */
  private Method22(src: number): number {
    return src * 2;
  }
  public Method2(param1: string | number): number {
    if (typeof param1 === 'string') {
      this.Method21(param1);
      return;
    }
    if (typeof param1 === 'number') {
      this.Method22(param1);
      return;
    }
    throw new Error('Error');
  }
  /**
   * コンストラクタ
   */
  private constructor1() {
    // ローカル変数宣言確認(型推論)
    let local: number = 123;
    // ローカル変数の値分岐
    if (local >= 10) {
      this.prop = local.toString();
      local = 1;
    }
    else {
      // ローカル変数宣言確認(型指定)
      let test: number = 0;
    }
    // プロパティの値分岐
    if (this.prop === "123") {
      let localString: string = "";
      localString = this.prop;
    }
    // ローカル変数でのswitch
    switch (local) {
      case 1:
        this.prop = "1";
        break;
      case 2:
        this.prop = "2";
        break;
      case 3:
      case 4:
        this.prop = "34";
        break;
      default:
        this.prop = "333";
        break;
    }
    // for分確認
    for (let i: number = 0; i < 10; i++) {
      local = i;
      this.prop = local.toString();
    }
    // for分確認(プロパティ)
    for (let i: number = 0; i < this.propInt; i++) {
      local = i;
      this.prop = local.toString();
    }
    // 計算代入式1
    local += local * 3;
    // 計算代入式2
    local -= local / this.propInt;
    local = local.toString().length;
  }
  /**
   * 複数コンストラクタ1
   * @param paramValue パラメータ
   */
  private constructor2(paramValue: number) {
    this.prop = paramValue.toString();
  }
  /**
   * 複数コンストラクタ2
   * @param param パラメータ
   */
  private constructor3(param: string) {
    this.prop = "コンストラクタ";
  }
  /**
   * 複数コンストラクタ3
   * @param param パラメータ1
   * @param boolValue パラメータ2
   */
  private constructor4(param: string, boolValue: boolean) {
    this.prop = "コンストラクタ";
  }
  /**
   * 複数コンストラクタ4
   * @param param パラメータ1
   * @param dateValue パラメータ2
   */
  private constructor5(param: string, dateValue: Date) {
    param = "コンストラクタ";
  }
  constructor(param1?: number | string, param2?: boolean | Date) {
    if (param1 === undefined && param2 === undefined) {
      this.constructor1();
      return;
    }
    if (typeof param1 === 'number' && param2 === undefined) {
      this.constructor2(param1);
      return;
    }
    if (typeof param1 === 'string' && param2 === undefined) {
      this.constructor3(param1);
      return;
    }
    if (typeof param1 === 'string' && typeof param2 === 'boolean') {
      this.constructor4(param1, param2);
      return;
    }
    if (typeof param1 === 'string' && param2 instanceof Date) {
      this.constructor5(param1, param2);
      return;
    }
  }
}
