export interface UserCreateDto {
  name: string;
  role: 'User' | 'Admin';
}

export interface UserLoginDto {
  name: string;
  role: 'User' | 'Admin';
}

export interface UserReadDto {
  id: number;
  name: string;
  role: 'User' | 'Admin';
}
