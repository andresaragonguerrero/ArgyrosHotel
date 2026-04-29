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
  @ViewChild('viewport', { static: false }) viewport!: ElementRef<HTMLDivElement>;
  @ViewChild('entViewport', { static: false }) entViewport!: ElementRef<HTMLUListElement>;

  isAtStart = true;
  isAtEnd = false;
  isAtStartEnt = true;
  isAtEndEnt = false;

  scrollProgress = 0;
  private isDragging = false;
  private startX = 0;
  private scrollLeftStart = 0;

  // código para el overline
  @ViewChild('overline', { static: false })
  overline!: ElementRef<HTMLElement>;

  constructor(private readonly cdr: ChangeDetectorRef) { }

  ngAfterViewInit() {
    this.initDragScroll();
    this.updateButtonState();
    this.updateButtonStateEnt();

    // código para el overview
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          this.overline.nativeElement.classList.add('is-visible');
          observer.disconnect();
        }
      },
      { threshold: 0.5 }
    );

    observer.observe(this.overline.nativeElement);
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
    const card = el.querySelector('.rooms__card') as HTMLElement;
    if (!card) return;

    const gap = Number(getComputedStyle(el.querySelector('.rooms__list')!).gap) || 0;
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

  scrollNextEnt() {
    this.doScrollEnt(1);
  }

  scrollPrevEnt() {
    this.doScrollEnt(-1);
  }

  private doScrollEnt(direction: number) {
    const el = this.entViewport.nativeElement;
    const card = el.querySelector('.entertainment__item') as HTMLElement;
    if (!card) return;

    const gap = Number(getComputedStyle(el).gap.replace('px', '')) || 0;
    const scrollAmount = (card.offsetWidth + gap) * direction;

    el.scrollBy({
      left: scrollAmount,
      behavior: 'smooth'
    });
  }

  updateButtonStateEnt() {
    if (!this.entViewport) return;

    const el = this.entViewport.nativeElement;
    this.isAtStartEnt = el.scrollLeft <= 2;
    this.isAtEndEnt = el.scrollLeft + el.clientWidth >= el.scrollWidth - 2;
    this.cdr.detectChanges();
  }

  setActiveEvent(index: number) {
  const items = document.querySelectorAll('.events__carousel-item');

  items.forEach((el, i) => {
    el.classList.toggle('is-active', i === index);
  });
}
}