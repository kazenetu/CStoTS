import { String } from '../../String';

export class InitializeResponse extends ResponseBase<InitializeResponse.InitializeResponseParam> {
  private constructor1() {
    base();
  }
  private constructor2(result: Results, errorMessage: string) {
    base(result, errorMessage);
  }
  private constructor3(result: Results, errorMessage: string, responseData: InitializeResponse.InitializeResponseParam) {
    base(result, errorMessage, responseData);
  }
  constructor(param1?: Results, param2?: string, param3?: InitializeResponse.InitializeResponseParam) {
    if (param1 === undefined && param2 === undefined && param3 === undefined) {
      this.constructor1();
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 === undefined) {
      this.constructor2(param1, param2);
      return;
    }
    if (param1 instanceof Results && typeof param2 === 'string' && param3 instanceof InitializeResponse.InitializeResponseParam) {
      this.constructor3(param1, param2, param3);
      return;
    }
  }
}
export namespace InitializeResponse {
  export class InitializeResponseParam {
    private _OrderUserID_: string = '';
    /**
     * 注文者ID
     */
    public set OrderUserID(value: string) {
      this._OrderUserID_ = value;
    }
    /**
     * 注文者ID
     */
    public get OrderUserID(): string {
      return this._OrderUserID_;
    }
    private _ModVersion_: number;
    /**
     * 更新バージョン
     */
    public set ModVersion(value: number) {
      this._ModVersion_ = value;
    }
    /**
     * 更新バージョン
     */
    public get ModVersion(): number {
      return this._ModVersion_;
    }
  }
}
