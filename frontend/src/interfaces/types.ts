export interface Note {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}

export interface User {
  username: string;
  token: string;
}

export interface LoginCredentials {
  username: string;
  password: string;
}

export type SortField = "createdAt" | "title" | "updatedAt";
export type SortDirection = "asc" | "desc";

export interface NoteFormData {
  title: string;
  content: string;
}
