export class FindUserNameResponse extends ResponseBase<FindUserNameResponse.FindUserNameResponseParam> {
  private constructor1() {
    base();
  }
  private constructor2(result: Results, errorMessage: string) {
    base(result, errorMessage);
  }
  private constructor3(result: Results, errorMessage: string, responseData: FindUserNameResponse.FindUserNameResponseParam) {
    base(result, errorMessage, responseData);
  }
  constructor(param1?: Results, param2?: string, param3?: FindUserNameResponse.FindUserNameResponseParam) {
    if (param1 === undefined && param2 === undefined && param3 === undefined) {
      this.constructor1();
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 === undefined) {
      this.constructor2(param1, param2);
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 instanceof FindUserNameResponse.FindUserNameResponseParam) {
      this.constructor3(param1, param2, param3);
      return;
    }
  }
}
export namespace FindUserNameResponse {
  export class FindUserNameResponseParam {
    private _OrderUserName_: string;
    /**
     * 注文者名
     */
    public set OrderUserName(value: string) {
      this._OrderUserName_ = value;
    }
    /**
     * 注文者名
     */
    public get OrderUserName(): string {
      return this._OrderUserName_;
    }
  }
}
