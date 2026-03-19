import { Pet } from './pet.model';

export interface Owner {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  address: string;
  city: string;
  createdAt: string;
  updatedAt: string;
  pets?: Pet[];
}

export interface CreateOwner {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  address: string;
  city: string;
}

export interface UpdateOwner extends CreateOwner {
  id: number;
}
