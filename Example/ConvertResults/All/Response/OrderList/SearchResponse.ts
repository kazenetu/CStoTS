import { Dictionary } from '../../Dictionary';
import { List } from '../../List';
import { CustomTOrder } from '../CustomTables/CustomTOrder';

export class SearchResponse extends ResponseBase<SearchResponse.SearchResponseParam> {
  private constructor1() {
    base();
  }
  private constructor2(result: Results, errorMessage: string) {
    base(result, errorMessage);
  }
  private constructor3(result: Results, errorMessage: string, responseData: SearchResponse.SearchResponseParam) {
    base(result, errorMessage, responseData);
  }
  constructor(param1?: Results, param2?: string, param3?: SearchResponse.SearchResponseParam) {
    if (param1 === undefined && param2 === undefined && param3 === undefined) {
      this.constructor1();
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 === undefined) {
      this.constructor2(param1, param2);
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 instanceof SearchResponse.SearchResponseParam) {
      this.constructor3(param1, param2, param3);
      return;
    }
  }
}
export namespace SearchResponse {
  export class SearchResponseParam {
    private _Results_: List<CustomTOrder> = new List<CustomTOrder>();
    /**
     * 検索結果
     */
    public get Results(): List<CustomTOrder> {
      return this._Results_;
    }
    private _dt_: Date;
    // TypeScript変換確認用
    public set dt(value: Date) {
      this._dt_ = value;
    }
    // TypeScript変換確認用
    public get dt(): Date {
      return this._dt_;
    }
    private _test_: Dictionary<number, number>;
    public set test(value: Dictionary<number, number>) {
      this._test_ = value;
    }
    public get test(): Dictionary<number, number> {
      return this._test_;
    }
    private _test2_: Dictionary<number, List<string>>;
    public set test2(value: Dictionary<number, List<string>>) {
      this._test2_ = value;
    }
    public get test2(): Dictionary<number, List<string>> {
      return this._test2_;
    }
    private _a_: number?;
    public set a(value: number?) {
      this._a_ = value;
    }
    public get a(): number? {
      return this._a_;
    }
    private _b_: CustomTOrder;
    public set b(value: CustomTOrder) {
      this._b_ = value;
    }
    public get b(): CustomTOrder {
      return this._b_;
    }
    private _c_: number = 1;
    public set c(value: number) {
      this._c_ = value;
    }
    public get c(): number {
      return this._c_;
    }
    private _c1_: float;
    public set c1(value: float) {
      this._c1_ = value;
    }
    public get c1(): float {
      return this._c1_;
    }
    private _c2_: number;
    public set c2(value: number) {
      this._c2_ = value;
    }
    public get c2(): number {
      return this._c2_;
    }
    private _c3_: number;
    public set c3(value: number) {
      this._c3_ = value;
    }
    public get c3(): number {
      return this._c3_;
    }
    private _bl_: boolean;
    public set bl(value: boolean) {
      this._bl_ = value;
    }
    public get bl(): boolean {
      return this._bl_;
    }
    private _s_: string = "123";
    public set s(value: string) {
      this._s_ = value;
    }
    public get s(): string {
      return this._s_;
    }
    private _li_: List<number> = new List<number>();
    public set li(value: List<number>) {
      this._li_ = value;
    }
    public get li(): List<number> {
      return this._li_;
    }
    private _nb_: boolean? = true;
    public set nb(value: boolean?) {
      this._nb_ = value;
    }
    public get nb(): boolean? {
      return this._nb_;
    }
    private _nli_: List<number?> = new List<number?>();
    public set nli(value: List<number?>) {
      this._nli_ = value;
    }
    public get nli(): List<number?> {
      return this._nli_;
    }
  }
}
