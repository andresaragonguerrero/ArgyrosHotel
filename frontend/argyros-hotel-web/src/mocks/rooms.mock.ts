import singleImg from "../assets/images/rooms/single-room.jpg";
import doubleImg from "../assets/images/rooms/double-room.jpg";
import familyImg from "../assets/images/rooms/family-room.jpg";
import deluxeImg from "../assets/images/rooms/deluxe-room.jpg";
import suiteImg from "../assets/images/rooms/suite.jpg";

export const mockRooms = [
  {
    id: "1",
    name: "Habitación individual",
    basePrice: 60,
    capacity: 1,
    totalRooms: 5,
    imageUrl: singleImg,
    description:
      "Habitación pensada para el viajero que busca comodidad sin excesos. Cuenta con una cama individual, escritorio de trabajo y baño privado. Su distribución compacta aprovecha cada rincón, ofreciendo un espacio funcional y luminoso, ideal para estancias cortas de negocios o descanso puntual durante un viaje.",
    squareMeters: 18,
  },
  {
    id: "2",
    name: "Habitación doble",
    basePrice: 90,
    capacity: 2,
    totalRooms: 8,
    imageUrl: doubleImg,
    description:
      "Espacio amplio y luminoso equipado con cama doble o dos individuales, zona de descanso independiente y baño completo. Su diseño equilibra confort y funcionalidad, siendo la opción preferida por parejas o compañeros de viaje que buscan una estancia agradable sin renunciar al espacio personal.",
    squareMeters: 26,
  },
  {
    id: "3",
    name: "Habitación familiar",
    basePrice: 130,
    capacity: 4,
    totalRooms: 4,
    imageUrl: familyImg,
    description:
      "Diseñada para familias, combina una cama doble con literas o camas adicionales en una distribución que separa las zonas de descanso. Ofrece espacio de sobra para el equipaje y el juego, además de un baño amplio, garantizando comodidad para todos los miembros durante toda la estancia.",
    squareMeters: 35,
  },
  {
    id: "4",
    name: "Habitación deluxe",
    basePrice: 160,
    capacity: 2,
    totalRooms: 3,
    imageUrl: deluxeImg,
    description:
      "Habitación superior con acabados de mayor calidad, zona de estar independiente y vistas privilegiadas. Incluye amenities exclusivos y un baño con bañera o ducha de diseño. Pensada para quienes buscan una experiencia más elevada, combinando amplitud, estilo y detalles que marcan la diferencia frente a las habitaciones estándar.",
    squareMeters: 32,
  },
  {
    id: "5",
    name: "Suite",
    basePrice: 220,
    capacity: 2,
    totalRooms: 2,
    imageUrl: suiteImg,
    description:
      "La máxima expresión de confort del hotel: salón separado del dormitorio, terraza privada y baño de lujo con bañera de hidromasaje. Cada detalle está cuidado para ofrecer una estancia excepcional, ideal para ocasiones especiales o huéspedes que desean disfrutar de un espacio exclusivo y espacioso.",
    squareMeters: 48,
  },
];
