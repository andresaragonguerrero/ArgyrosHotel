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

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.header__lang')) {
      this.isOpen = false;
    }
  }

  toggleDark() {
    this.isDark = !this.isDark;
    document.documentElement.classList.toggle('dark', this.isDark);
    localStorage.setItem('theme', this.isDark ? 'dark' : 'light');
  }
}