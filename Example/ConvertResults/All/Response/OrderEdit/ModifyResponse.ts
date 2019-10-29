export class ModifyResponse extends ResponseBase<ModifyResponse.ModifyResponseParam> {
  private constructor1() {
    base();
  }
  private constructor2(result: Results, errorMessage: string) {
    base(result, errorMessage);
  }
  private constructor3(result: Results, errorMessage: string, responseData: ModifyResponse.ModifyResponseParam) {
    base(result, errorMessage, responseData);
  }
  constructor(param1?: Results, param2?: string, param3?: ModifyResponse.ModifyResponseParam) {
    if (param1 === undefined && param2 === undefined && param3 === undefined) {
      this.constructor1();
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 === undefined) {
      this.constructor2(param1, param2);
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 instanceof ModifyResponse.ModifyResponseParam) {
      this.constructor3(param1, param2, param3);
      return;
    }
  }
}
export namespace ModifyResponse {
  export class ModifyResponseParam {
    private _OrderNo_: number;
    /**
     * 注文番号
     */
    public set OrderNo(value: number) {
      this._OrderNo_ = value;
    }
    /**
     * 注文番号
     */
    public get OrderNo(): number {
      return this._OrderNo_;
    }
  }
}
