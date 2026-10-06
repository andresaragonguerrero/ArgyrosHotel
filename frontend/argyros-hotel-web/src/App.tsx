import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { Navbar } from "./components/Navbar";
import { RoomsPage } from "./pages/RoomsPage";
import { ServicesPage } from "./pages/ServicesPage";
import { BookingsPage } from "./pages/BookingsPage";
import { RegisterPage } from "./pages/RegisterPage";
import { LoginPage } from "./pages/LoginPage";
import { BookingDatesPage } from "./pages/booking/BookingDatesPage";
import { BookingRoomsPage } from "./pages/booking/BookingRoomsPage";
import { BookingServicesPage } from "./pages/booking/BookingServicesPage";
import { BookingDataPage } from "./pages/booking/BookingDataPage";
import { BookingConfirmationPage } from "./pages/booking/BookingConfirmationPage";
import { BookingReceiptPage } from "./pages/booking/BookingReceiptPage";

export function App() {
  return (
    <Router>
      <Navbar />
      <main>
        <Routes>
          <Route path="/" element={<Navigate to="/rooms" replace />} />
          <Route path="/rooms" element={<RoomsPage />} />
          <Route path="/services" element={<ServicesPage />} />
          <Route path="/bookings" element={<BookingsPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/booking/dates" element={<BookingDatesPage />} />
          <Route path="/booking/rooms" element={<BookingRoomsPage />} />
          <Route path="/booking/services" element={<BookingServicesPage />} />
          <Route path="/booking/data" element={<BookingDataPage />} />
          <Route
            path="/booking/confirmation"
            element={<BookingConfirmationPage />}
          />
          <Route path="/booking/receipt/:id" element={<BookingReceiptPage />} />
        </Routes>
      </main>
    </Router>
  );
}

export default App;
