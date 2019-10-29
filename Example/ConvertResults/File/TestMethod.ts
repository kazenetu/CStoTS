/**
 * メソッドテストクラス
 */
export class TestMethod {
  public static StaticMethod(): string {
    return "ccc";
  }
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
    // this.prop = TestMethod.StaticMethod()
    this.prop = TestMethod.StaticMethod();
    // this.prop = TestLogic.StaticMethod()
    this.prop = TestMethod.StaticMethod();
    // this.prop = TestMethod.InnerClass.StaticMethod()
    this.prop = TestMethod.InnerClass.StaticMethod();
    // this.prop = TestMethod.InnerClass.StaticMethod()
    this.prop = TestMethod.InnerClass.StaticMethod();
    // this.prop = TestMethod.InnerClass.StaticMethodArg("test")
    this.prop = TestMethod.InnerClass.StaticMethodArg("test");
    // this.prop = TestMethod.InnerClass.StaticMethodArg(propInt.toString())
    this.prop = TestMethod.InnerClass.StaticMethodArg(this.propInt.toString());
    // this.prop = OtherClass.StaticMethod()
    this.prop = OtherClass.StaticMethod();
    // this.prop = OtherClass.InnerClass.StaticMethod()
    this.prop = OtherClass.InnerClass.StaticMethod();
    // this.prop = OtherClassc.InnerClass.StaticMethodArg("test")
    this.prop = OtherClass.InnerClass.StaticMethodArg("test");
    // this.prop = OtherClass.InnerClass.StaticMethodArg(propInt.toString())
    this.prop = OtherClass.InnerClass.StaticMethodArg(this.propInt.toString());
  }
}
export namespace TestMethod {
  export class InnerClass {
    public static StaticField: string = "789";
    public static StaticMethod(): string {
      return "bbb";
    }
    public static StaticMethodArg(name: string): string {
      return "hey!" + name;
    }
  }
}
