export interface ApiResponse<T> {
  errorMessage: string;
  timeGenerated: Date;
  result: T;
}