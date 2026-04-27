import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  isOpen = false;
  isDropdownOpen = false;
  isSidebarOpen = false;

  languages = ['Español', 'Francés', 'Inglés'];
  selectedLanguage = 'Español';
  isDark = localStorage.getItem('theme') === 'dark';

  constructor() {
    document.documentElement.classList.toggle('dark', this.isDark);
  }

  toggleLang() {
    this.isOpen = !this.isOpen;
  }

  selectLang(language: string) {
    this.selectedLanguage = language;
    this.isOpen = false;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
    this.isDropdownOpen = false; // cierra el otro
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
    this.isSidebarOpen = false; // cierra el otro
  }

  closeDropdown() {
    this.isDropdownOpen = false;
    this.updateBodyScroll();
  }

  closeSidebar() {
    this.isSidebarOpen = false;
    this.updateBodyScroll();
  }

  private updateBodyScroll() {
    document.body.style.overflow = this.isDropdownOpen ? 'hidden' : '';
  }

  @HostListener('document:click', ['$event'])
  onGlobalClick(event: MouseEvent) {
    const target = event.target as HTMLElement;

    if (this.isOpen && !target.closest('.header__lang')) {
      this.isOpen = false;
    }

    if (this.isDropdownOpen &&
      !target.closest('.header__dropdown-menu') &&
      !target.closest('.header__dropdown-trigger')) {
      this.closeDropdown();
    }

    if (this.isSidebarOpen &&
      !target.closest('.header__sidebar') &&
      !target.closest('.header__sidebar-trigger')) {
      this.closeSidebar();
    }
  }

  @HostListener('document:keydown.escape')
  onEscape() {
    this.isOpen = false;
    this.isSidebarOpen = false;
    this.isDropdownOpen = false;
  }

  toggleDark() {
    this.isDark = !this.isDark;
    document.documentElement.classList.toggle('dark', this.isDark);
    localStorage.setItem('theme', this.isDark ? 'dark' : 'light');
  }
}