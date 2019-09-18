/**
 * Simple JSONConverter for TypeScript
 * Copyright 2019, kazenetu
 * Released under the MIT License.
 */
export class JSONConverter<T>
{
  public serialize(target: T): string {
    return JSON.stringify(target);
  }
  public deserialize(jsonString: string, newInstance: T): T {
    return this.deserializeMapping(newInstance, JSON.parse(jsonString));
  }
  private deserializeMapping(newInstance: any, jsonObject: any): T {
    for (let itemName in jsonObject) {
      let itemObject = jsonObject[itemName];

      if (typeof itemObject === 'object') {
        this.deserializeMapping(newInstance[itemName], itemObject);
      } else {
        newInstance[itemName] = jsonObject[itemName];
      }
    }
    return newInstance;
  }
}