export interface Pagination<T> {
    pageNumber: number
    pageSize: number
    count: number
    sortOrder: string
    data: T
  }