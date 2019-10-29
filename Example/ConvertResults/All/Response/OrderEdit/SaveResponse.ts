export class SaveResponse extends ResponseBase<SaveResponse.SaveResponseParam> {
  private constructor1() {
    base();
  }
  private constructor2(result: Results, errorMessage: string) {
    base(result, errorMessage);
  }
  private constructor3(result: Results, errorMessage: string, responseData: SaveResponse.SaveResponseParam) {
    base(result, errorMessage, responseData);
  }
  constructor(param1?: Results, param2?: string, param3?: SaveResponse.SaveResponseParam) {
    if (param1 === undefined && param2 === undefined && param3 === undefined) {
      this.constructor1();
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 === undefined) {
      this.constructor2(param1, param2);
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 instanceof SaveResponse.SaveResponseParam) {
      this.constructor3(param1, param2, param3);
      return;
    }
  }
}
export namespace SaveResponse {
  export class SaveResponseParam {
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
