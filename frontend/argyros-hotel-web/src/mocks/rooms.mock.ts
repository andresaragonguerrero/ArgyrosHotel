import singleImg from "../assets/images/rooms/single-room.jpg";
import doubleImg from "../assets/images/rooms/double-room.jpg";
import familyImg from "../assets/images/rooms/family-room.jpg";
import deluxeImg from "../assets/images/rooms/deluxe-room.jpg";
import suiteImg from "../assets/images/rooms/suite.jpg";

export const mockRooms = [
  {
    id: "1",
    name: "Single Room",
    basePrice: 60,
    capacity: 1,
    totalRooms: 5,
    imageUrl: singleImg,
  },
  {
    id: "2",
    name: "Double Room",
    basePrice: 90,
    capacity: 2,
    totalRooms: 8,
    imageUrl: doubleImg,
  },
  {
    id: "3",
    name: "Family Room",
    basePrice: 130,
    capacity: 4,
    totalRooms: 4,
    imageUrl: familyImg,
  },
  {
    id: "4",
    name: "Deluxe Room",
    basePrice: 160,
    capacity: 2,
    totalRooms: 3,
    imageUrl: deluxeImg,
  },
  {
    id: "5",
    name: "Suite Room",
    basePrice: 220,
    capacity: 2,
    totalRooms: 2,
    imageUrl: suiteImg,
  },
];
