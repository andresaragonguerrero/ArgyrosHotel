import { useState, useEffect } from "react";
import type { BookingDraft } from "../types/bookingDraft";
import { EMPTY_DRAFT } from "../types/bookingDraft";

const STORAGE_KEY = "argyros_booking_draft";

export const useBookingDraft = () => {
  const [draft, setDraft] = useState<BookingDraft>(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    return stored ? JSON.parse(stored) : EMPTY_DRAFT;
  });

  useEffect(() => {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(draft));
  }, [draft]);

  const updateDraft = (partial: Partial<BookingDraft>) => {
    setDraft((prev) => ({ ...prev, ...partial }));
  };

  const clearDraft = () => {
    setDraft(EMPTY_DRAFT);
    localStorage.removeItem(STORAGE_KEY);
  };

  return { draft, updateDraft, clearDraft };
};
