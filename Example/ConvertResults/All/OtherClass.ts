export class OtherClass {
  public static StaticField: string = "456";
  public static StaticMethod(): string {
    return "bbb";
  }
}
export namespace OtherClass {
  export class InnerClass {
    public static StaticField: string = "789";
    public static StaticMethod(): string {
      return "aaa";
    }
    public static StaticMethodArg(name: string): string {
      return "hellow!" + name;
    }
  }
}
