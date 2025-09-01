export interface ResponseGeneric<T> {
    status: number,
    message: any,
    result: T
}

export interface TokenResponse {
  token: string;
  expiresIn: number;
  issuedAt: Date;
}