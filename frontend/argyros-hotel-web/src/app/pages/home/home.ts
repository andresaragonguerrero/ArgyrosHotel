import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, ViewChild } from '@angular/core';
// Components
import { Header } from '../../shared/components/header/header';
import { Footer } from '../../shared/components/footer/footer';

@Component({
  selector: 'app-home',
  imports: [
    Header,
    Footer,
  ],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements AfterViewInit {
  // código para el carrusel
  @ViewChild('viewport', { static: false })
  viewport!: ElementRef<HTMLDivElement>;
  isAtStart = true;
  isAtEnd = false;
  scrollProgress = 0;
  private isDragging = false;
  private startX = 0;
  private scrollLeftStart = 0;

  constructor(private readonly cdr: ChangeDetectorRef) { }

  ngAfterViewInit() {
    this.initDragScroll();
    this.updateButtonState();
  }

  private initDragScroll() {
    const el = this.viewport.nativeElement;

    el.addEventListener('mousedown', (e) => {
      this.isDragging = true;
      el.classList.add('grabbing');
      this.startX = e.pageX - el.offsetLeft;
      this.scrollLeftStart = el.scrollLeft;
    });

    el.addEventListener('mouseleave', () => {
      this.isDragging = false;
      el.classList.remove('grabbing');
    });

    el.addEventListener('mouseup', () => {
      this.isDragging = false;
      el.classList.remove('grabbing');
    });

    el.addEventListener('mousemove', (e) => {
      if (!this.isDragging) return;
      e.preventDefault();
      const x = e.pageX - el.offsetLeft;
      const walk = (x - this.startX) * 1.5;
      el.scrollLeft = this.scrollLeftStart - walk;
    });
  }

  scrollNext() {
    this.doScroll(1);
  }

  scrollPrev() {
    this.doScroll(-1);
  }

  private doScroll(direction: number) {
    const el = this.viewport.nativeElement;
    const card = el.querySelector('.services__card') as HTMLElement;
    if (!card) return;

    const gap = Number(getComputedStyle(el.querySelector('.services__list')!).gap) || 0;
    const scrollAmount = (card.offsetWidth + gap) * direction;

    el.scrollBy({
      left: scrollAmount,
      behavior: 'smooth'
    });
  }

  updateScrollProgress() {
    if (!this.viewport) return;

    const el = this.viewport.nativeElement;
    const maxScroll = el.scrollWidth - el.clientWidth;

    if (maxScroll <= 0) {
      this.scrollProgress = 0;
    } else {
      this.scrollProgress = (el.scrollLeft / maxScroll) * 100;
    }

    this.updateButtonState();
  }

  updateButtonState() {
    const el = this.viewport.nativeElement;
    this.isAtStart = el.scrollLeft <= 2;
    this.isAtEnd = el.scrollLeft + el.clientWidth >= el.scrollWidth - 2;
    this.cdr.detectChanges();
  }
}