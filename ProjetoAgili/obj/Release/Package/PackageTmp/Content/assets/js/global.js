var MINOVATE = MINOVATE || {};

$(function() {




  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // global inicialization functions
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.global = {

    init: function() {
      MINOVATE.global.deviceSize();
      MINOVATE.global.goToTop();
      MINOVATE.global.animsition();
    },

    // device identification function
    deviceSize: function(){
			var jRes = jRespond([
				{
					label: 'smallest',
					enter: 0,
					exit: 479
				},{
					label: 'handheld',
					enter: 480,
					exit: 767
				},{
					label: 'tablet',
					enter: 768,
					exit: 991
				},{
					label: 'laptop',
					enter: 992,
					exit: 1199
				},{
					label: 'desktop',
					enter: 1200,
					exit: 10000
				}
			]);
			jRes.addFunc([
				{
					breakpoint: 'desktop',
					enter: function() { $body.addClass('device-lg'); },
					exit: function() { $body.removeClass('device-lg'); }
				},{
					breakpoint: 'laptop',
					enter: function() { $body.addClass('device-md'); },
					exit: function() { $body.removeClass('device-md'); }
				},{
					breakpoint: 'tablet',
					enter: function() { $body.addClass('device-sm'); },
					exit: function() { $body.removeClass('device-sm'); }
				},{
					breakpoint: 'handheld',
					enter: function() { $body.addClass('device-xs'); },
					exit: function() { $body.removeClass('device-xs'); }
				},{
					breakpoint: 'smallest',
					enter: function() { $body.addClass('device-xxs'); },
					exit: function() { $body.removeClass('device-xxs'); }
				}
			]);
		},

    // scroll to top
    goToTop: function(){
			$goToTopEl.click(function() {
				$('body,html').stop(true).animate({scrollTop:0},400);
				return false;
			});
		},

		// show gotoTop element
    goToTopScroll: function(){
			if( $body.hasClass('device-lg') || $body.hasClass('device-md') || $body.hasClass('device-sm') ) {
				if($window.scrollTop() > 450) {
					$goToTopEl.fadeIn();
				} else {
					$goToTopEl.fadeOut();
				}
			}
		},

    // initialize animsition

    animsition: function() {
      $wrapper.animsition({
        inClass               :   'fade-in',
        outClass              :   'fade-out',
        inDuration            :    1500,
        outDuration           :    800,
        linkElement           :   '.animsition-link',
        // e.g. linkElement   :   'a:not([target="_blank"]):not([href^=#])'
        loading               :    true,
        loadingParentElement  :   'body', //animsition wrapper element
        loadingClass          :   'animsition-loading',
        unSupportCss          : [ 'animation-duration',
          '-webkit-animation-duration',
          '-o-animation-duration'
        ],
        //"unSupportCss" option allows you to disable the "animsition" in case the css property in the array is not supported by your browser.
        //The default setting is to disable the "animsition" in a browser that does not support "animation-duration".

        overlay               :   false,
        overlayClass          :   'animsition-overlay-slide',
        overlayParentElement  :   'body'
      });
    }

  };






  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // header section functions
  //!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.header = {

    init: function() {
      MINOVATE.header.superfish();
      MINOVATE.header.megaMenuWidth();
      MINOVATE.header.searchToggle();
      MINOVATE.header.chooseMenuStyle();
      MINOVATE.header.chooseLogo();
      MINOVATE.header.stickyHeader();
      MINOVATE.header.menuClasses();
      MINOVATE.header.toggleMenu();
      MINOVATE.header.shopCart();
    },


    //initialize superfish on main-navbar
    superfish: function(){

      if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
        $('#main-navbar ul ul, #main-navbar ul .mega-menu-content, #add-navbar ul ul').css('display', 'block');
			  MINOVATE.header.dropdownPosition();
      }

			$('#main-navbar > ul, #main-navbar > div > ul, #add-navbar .container > ul').superfish({
        popUpSelector: 'ul,.mega-menu-content',
        delay: 250,
        speed: 350,
        animation: {opacity:'show'},
        animationOut:  {opacity:'hide'},
        cssArrows: false
      });

		},

    //choose side for dropdown menu open
    dropdownPosition: function() {

			$('#main-navbar .mega-menu-content, #main-navbar ul ul, #add-navbar ul ul').each( function( index, element ){
				var $dropdown = $(element);
				var windowWidth = $window.width();
				var dropdownOffset = $dropdown.offset();
				var dropdownWidth = $dropdown.width();
				var dropdownPosition = dropdownOffset.left;

				if(windowWidth - (dropdownWidth + dropdownPosition) < 0) {
					$dropdown.addClass('menu-right-position');
				}
			});

		},

    //set width of mega-menu
    megaMenuWidth: function(){

      $('.mega-menu .mega-menu-content').css({
        'width': $wrapper.width() - 60
      });

    },

    //toggle search box
    searchToggle: function(){

      $('#search-toggle a').click(function(e){
        e.preventDefault();

        if($searchBoxWrapper.height() == "0"){

          $('html, body').animate({
            scrollTop: 0
          }, 300, false);

          $('.search-box-wrapper input[type="search"]').focus();

          $searchBoxWrapper.animate({
            height:"50px"
          }, 300);

        } else {

          $searchBoxWrapper.animate({
            height:"0"
          }, 300);

        }

      });

    },

    // choose if logo need to be light or dark
    chooseLogo: function(){
			if($header.hasClass('light')) {
				if( defaultLightLogo ){ defaultLogo.find('img').attr('src', defaultLightLogo); }
				if( retinaLightLogo ){ retinaLogo.find('img').attr('src', retinaLightLogo); }
			} else {
				if( defaultLogoImg ){ defaultLogo.find('img').attr('src', defaultLogoImg); }
				if( retinaLogoImg ){ retinaLogo.find('img').attr('src', retinaLogoImg); }
			}
		},

    // makes header sticky
    stickyHeader: function( headerOffset ){
			if ($window.scrollTop() > headerOffset) {
				if( ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) && !MINOVATE.isMobile.any() ) {
					$header.addClass('sticky-header');
					$header.removeClass('light');
				} else if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm') ) {
          if( $header.hasClass('sticky-mobile') ) { $header.addClass('responsive-sticky-header'); }
				} else {
					MINOVATE.header.unStickyHeader();
				}
			} else {
				MINOVATE.header.unStickyHeader();
			}
		},

    // remove sticky class on header
    unStickyHeader: function(){
			if( $header.hasClass('sticky-header') ){
				$header.removeClass('sticky-header');
				$header.removeClass().addClass(oldHeaderClasses);
			}
			if( $header.hasClass('responsive-sticky-header') ){
				$header.removeClass('responsive-sticky-header');
			}
		},

    // class toggling on menu according to windows width
    chooseMenuStyle: function(headerOffset) {
      if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm')) {
        $header.removeClass('light');
        if( !$header.hasClass('sticky-mobile') ) { MINOVATE.header.unStickyHeader(); }
      } else {
        if( $window.scrollTop() > headerOffset ) { $header.addClass('sticky-header'); }
        if( !$header.hasClass('sticky-header')) { $header.addClass('light'); }
        if( $header.hasClass('dark')) { $header.removeClass('light'); }
      }
    },

    // add sub-menu class to determined menu items
    menuClasses: function() {
      $( '#main-navbar ul li:has(ul)' ).addClass('submenu');

      if( MINOVATE.isMobile.Android() ) {
				$( '#main-navbar ul li.sub-menu' ).children('a').on('touchstart', function(e){
					if( !$(this).parent('li.submenu').hasClass('sfHover') ) {
						e.preventDefault();
					}
				});
			}
    },

    // toggling display of menu
    toggleMenu: function() {
      $menuToggler.click(function() {
				$( '#main-navbar > ul' ).toggleClass("show");
				return false;
			});
    },

    // toggling shopping cart display
    shopCart: function(){

      $(document).on('click', function(event) {
				if (!$(event.target).closest('#shopping-cart').length) { $shopCart.toggleClass('shopping-cart-open', false); }
			});

			$("#shopping-cart-trigger").click(function(e){
				$shopCart.toggleClass('shopping-cart-open');
				e.stopPropagation();
				e.preventDefault();
			});

		}


  };
  
  
  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // slider section functions
  //!!!!!!!!!!!!!!!!!!!!!!!!!
  
  MINOVATE.slider = {

		init: function() {

			MINOVATE.slider.sliderParallax();

		},

		//set parallax offset
    sliderParallaxOffset: function(){
			var sliderParallaxOffsetTop = 0;
			var headerHeight = $header.outerHeight();
			if( $body.hasClass('side-header') || $header.hasClass('transparent-header') ) { headerHeight = 0; }
			if( $pageTitle.length > 0 ) {
				var pageTitleHeight = $pageTitle.outerHeight();
				sliderParallaxOffsetTop = pageTitleHeight + headerHeight;
			} else {
				sliderParallaxOffsetTop = headerHeight;
			}

			if( $slider.next('#header').length > 0 ) { sliderParallaxOffsetTop = 0; }

			return sliderParallaxOffsetTop;
		},

		//parallax transform function
    sliderParallax: function(){
			if( ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) && !MINOVATE.isMobile.any() ) {
				var parallaxOffsetTop = MINOVATE.slider.sliderParallaxOffset();
				if ($window.scrollTop() > parallaxOffsetTop) {
					$sliderParallaxEl.css({ 'transform' : 'translate(0,'+ (($window.scrollTop()-parallaxOffsetTop) / 1.5 ) +'px)' });
				} else {
					$('.slider-parallax').css({ 'transform' : 'translate(0,0)' });
				}
			} else {
				$('.slider-parallax').css({ 'transform' : 'translate(0,0)' });
			}
		}

		
	};





  //!!!!!!!!!!!!!!!!
  // extra functions
  //!!!!!!!!!!!!!!!!

  MINOVATE.extra = {

    init: function() {
      MINOVATE.extra.animations();
      MINOVATE.extra.parallax();
      MINOVATE.extra.loadFlexSlider();
      MINOVATE.extra.lightbox();
      MINOVATE.extra.flickrFeed();
      MINOVATE.extra.twitterFeed();
      MINOVATE.extra.html5Video();
      MINOVATE.extra.counter();
      MINOVATE.extra.progress();
      MINOVATE.extra.mixitup();
      MINOVATE.extra.sidebarMenu();
      MINOVATE.extra.bsSelect()
      MINOVATE.extra.rangeSlider();
      MINOVATE.extra.touchSpin();
    },


    //initialize animations on elements
    animations: function(){

      var $animateEl = $('[data-animate]');

      if( $animateEl.length > 0 ){

        if( $body.hasClass('device-lg') || $body.hasClass('device-md') || $body.hasClass('device-sm') ){

          $animateEl.each(function(){
            var el = $(this),
							  delay = el.attr('data-delay'),
							  delayTime = 0;

						if( delay ) { delayTime = Number( delay ) + 500; } else { delayTime = 500; }

						if( !el.hasClass('animated') ) {
							el.addClass('not-animated');
							var elAnimation = el.attr('data-animate');
							el.appear(function () {
								setTimeout(function() {
									el.removeClass('not-animated').addClass( elAnimation + ' animated');
								}, delayTime);
							},{accX: 0, accY: -120},'easeInCubic');
						}

					});

				}

			}

		},

    // initialize parallax effect on parallax elements
    parallax: function(){
			if( !MINOVATE.isMobile.any() ){
				$.stellar({
					horizontalScrolling: false,
					verticalOffset: 150,
					responsive: true
				});
			} else {
				$parallaxEl.addClass('mobile-parallax');
				$parallaxPageTitleEl.addClass('mobile-parallax');
			}
		},

    // initialize flexslider
    loadFlexSlider: function(){
			var $flexSliderEl = $('.fslider').find('.flexslider');

			if( $flexSliderEl.length > 0 ){
				$flexSliderEl.each(function() {
					var $flexsSlider = $(this),
              flexsAnimation = $flexsSlider.parent('.fslider').attr('data-animation'),
              flexsEasing = $flexsSlider.parent('.fslider').attr('data-easing'),
              flexsDirection = $flexsSlider.parent('.fslider').attr('data-direction'),
              flexsSlideshow = $flexsSlider.parent('.fslider').attr('data-slideshow'),
              flexsPause = $flexsSlider.parent('.fslider').attr('data-pause'),
              flexsSpeed = $flexsSlider.parent('.fslider').attr('data-speed'),
              flexsVideo = $flexsSlider.parent('.fslider').attr('data-video'),
              flexsPagi = $flexsSlider.parent('.fslider').attr('data-pagi'),
              flexsArrows = $flexsSlider.parent('.fslider').attr('data-arrows'),
              flexsThumbs = $flexsSlider.parent('.fslider').attr('data-thumbs'),
              flexsSheight = true,
              flexsUseCSS = false;

					if( !flexsAnimation ) { flexsAnimation = 'slide'; }
					if( !flexsEasing || flexsEasing == 'swing' ) {
						flexsEasing = 'swing';
						flexsUseCSS = true;
					}
					if( !flexsDirection ) { flexsDirection = 'horizontal'; }
					if( !flexsSlideshow ) { flexsSlideshow = true; } else { flexsSlideshow = false; }
					if( !flexsPause ) { flexsPause = 5000; }
					if( !flexsSpeed ) { flexsSpeed = 600; }
					if( !flexsVideo ) { flexsVideo = false; }
					if( flexsDirection == 'vertical' ) { flexsSheight = false; }
					if( flexsPagi == 'false' ) { flexsPagi = false; } else { flexsPagi = true; }
					if( flexsThumbs == 'true' ) { flexsPagi = 'thumbnails'; } else { flexsPagi = flexsPagi; }
					if( flexsArrows == 'false' ) { flexsArrows = false; } else { flexsArrows = true; }

					$flexsSlider.flexslider({
						selector: ".slider-wrap > .slide",
						animation: flexsAnimation,
						easing: flexsEasing,
						direction: flexsDirection,
						slideshow: flexsSlideshow,
						slideshowSpeed: Number(flexsPause),
						animationSpeed: Number(flexsSpeed),
						pauseOnHover: true,
						video: flexsVideo,
						controlNav: flexsPagi,
						directionNav: flexsArrows,
						smoothHeight: flexsSheight,
						useCSS: flexsUseCSS,
						start: function(slider){
							$('.flex-prev').html('<i class="fa fa-angle-left"></i>');
							$('.flex-next').html('<i class="fa fa-angle-right"></i>');
						}
					});
				});
			}
		},

    //initialize magnificPopup lightbox

    lightbox: function(){
			var $lightboxImageEl = $('[data-lightbox="image"]'),
          $lightboxIframeEl = $('[data-lightbox="iframe"]'),
          $lightboxGalleryEl = $('[data-lightbox="gallery"]');

			if( $lightboxImageEl.length > 0 ) {
				$lightboxImageEl.magnificPopup({
					type: 'image',
					closeOnContentClick: true,
					closeBtnInside: false,
					fixedContentPos: true,
					image: {
						verticalFit: true
					}
				});
			}

      if( $lightboxIframeEl.length > 0 ) {
				$lightboxIframeEl.magnificPopup({
					disableOn: 600,
					type: 'iframe',
					removalDelay: 160,
					preloader: false,
					fixedContentPos: false
				});
			}

			if( $lightboxGalleryEl.length > 0 ) {
				$lightboxGalleryEl.each(function() {
					var element = $(this);

					if( element.find('a[data-lightbox="gallery-item"]').parent('.clone').hasClass('clone') ) {
						element.find('a[data-lightbox="gallery-item"]').parent('.clone').find('a[data-lightbox="gallery-item"]').attr('data-lightbox','');
					}

					element.magnificPopup({
						delegate: 'a[data-lightbox="gallery-item"]',
						type: 'image',
						closeOnContentClick: true,
						closeBtnInside: false,
						fixedContentPos: true,
						image: {
							verticalFit: true
						},
						gallery: {
							enabled: true,
							navigateByImgClick: true,
							preload: [0,1] // Will preload 0 - before current, and 1 after the current image
						}
					});
				});
			}
		},

    //initialize FlickrFeed

    flickrFeed: function(){
			$('#flickr-feed').append('<ul class="photo-stream" data-lightbox="gallery"></ul>');

      $('#flickr-feed ul').jflickrfeed({
        limit: 12,
        qstrings: {
          id: '12625534@N00' // enter Flickr ID
        },

        itemTemplate: '<li><a href="{{image_b}}" title="{{title}}" data-lightbox="gallery-item"><img src="{{image_s}}" alt="{{title}}" /></a><span class="photo-stream-overlay"><i class="fa fa-arrows-alt"></i></span></li>'

      });

      MINOVATE.extra.lightbox();

		},

    //initialize Twitter Feed
    twitterFeed: function(){

      var initTweetFeed = function() {
        !function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0],p=/^http:/.test(d.location)?'http':'https';if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=p+"://platform.twitter.com/widgets.js";fjs.parentNode.insertBefore(js,fjs);}}(document,"script","twitter-wjs");
      }

      initTweetFeed();

		},

    //initialize resizing
    html5Video: function() {
      if ($body.hasClass('device-sm') || $body.hasClass('device-xs') || $body.hasClass('device-xxs')) {
        $('.section:has(video), .parallax-box:has(video)').each(function () {
          var outerContainerWidth = $(this).outerWidth();
          var outerContainerHeight = $(this).outerHeight();
          var innerVideoWidth = $(this).find('video').outerWidth();
          var innerVideoHeight = $(this).find('video').outerHeight();
          if (innerVideoHeight < outerContainerHeight) {
            var videoAspectRatio = innerVideoWidth / innerVideoHeight;
            var newVideoWidth = outerContainerHeight * videoAspectRatio;
            var innerVideoPosition = (newVideoWidth - outerContainerWidth) / 2;
            $(this).find('video').css({
              'width': newVideoWidth + 'px',
              'height': outerContainerHeight + 'px',
              'left': -innerVideoPosition + 'px',
              'position': 'relative'
            });
          } else {
            var innerVideoPosition = (innerVideoHeight - outerContainerHeight) / 2;
            $(this).find('video').css({
              'width': innerVideoWidth + 'px',
              'height': innerVideoHeight + 'px',
              'top': -innerVideoPosition + 'px',
              'position': 'relative'
            });
          }
        });
      } else {
        $('.section:has(video), .parallax-box:has(video)').each(function () {
          $(this).find('video').css({
            'width': '100%',
            'height': 'auto',
            'position': 'static'
          });
        });
      }
    },

    //initialize countTo
    counter: function(){
      var $counterEl = $('.counter:not(.counter-instant)');
			if( $counterEl.length > 0 ){
				$counterEl.each(function(){
					var element = $(this);
          element.appear( function(){
            MINOVATE.extra.runCounter(element);
          });
				});
			}
    },

    //run countTo
    runCounter: function( counterElement){
			counterElement.find('span').countTo();
		},

    //animate progress bar
    progress: function(){
			var $progressEl = $('.progress');
			if( $progressEl.length > 0 ){
				$progressEl.each(function(){
					var progressBar = $(this),
						  progressValue = progressBar.attr('data-percent');

					if( $body.hasClass('device-lg') || $body.hasClass('device-md') ){
						progressBar.appear( function(){
							if (!progressBar.hasClass('progress-animated')) {
								progressBar.find('.counter-instant span').countTo();
								progressBar.find('.progress-bar').css({width: progressValue + "%"}).addClass('progress-animated');
							}
						},{accX: 0, accY: -120},'easeInCubic');
					} else {
						progressBar.find('.counter-instant span').countTo();
						progressBar.find('.progress').css({width: progressValue + "%"});
					}
				});
			}
		},

    //initialize mixitup

    mixitup: function(){
			if( $mixitupEl.length > 0 ) {
				$mixitupEl.each(function() {
					var element = $(this);

					element.mixItUp({
            animation: {
              duration: 400,
              effects: 'translateZ(-360px) stagger(34ms) fade',
              easing: 'ease'
            }
          });
				});
			}
		},

    // toggling sidebar menu
    sidebarMenu: function(){

      $sidebarMenu.find( 'li:has(ul)' ).addClass('submenu');
      var submenu = $sidebarMenu.find('.submenu');
      submenu.prepend('<a href="#" class="dropdown-toggle"></a>');

      var dropdownToggle = $sidebarMenu.find('.dropdown-toggle');

      dropdownToggle.on('click', function(e) {

        var menuItem = $(this).parent();

        menuItem.toggleClass('open');
        menuItem.find('ul').addClass('animated fadeIn');
        e.stopPropagation();
				e.preventDefault();
      });

		},

    //initialize bootstrap select

    bsSelect: function(){
			if( $bsSelectEl.length > 0 ) {
				$bsSelectEl.each(function() {
					var element = $(this);

					element.selectpicker();
				});
			}
    },

    //initialize range slider

    rangeSlider: function(){
			if( $rangeSliderEl.length > 0 ) {
				$rangeSliderEl.each(function() {
					var element = $(this);
          var $rangeValue = $(this).closest('form').find('.range-value');
          var sliderUnit = element.data('sliderUnit');

          $rangeValue.html(sliderUnit + element.data('sliderValue')[0] + ' - ' + sliderUnit + element.data('sliderValue')[1]);

          element.slider()
              .on('slide', function(ev){
                $rangeValue.html(sliderUnit + ev.value[0] + ' - ' + sliderUnit + ev.value[1]);
              });
				});
			}
		},

    //initialize touchspin

    touchSpin: function(){
			if( $touchSpinEl.length > 0 ) {
				$touchSpinEl.each(function() {
					var element = $(this);

          element.TouchSpin({
            min: 1,
            max: 100,
            step: 1,
            buttondown_class: "myBtn myBtn-border myBtn-rounded myBtn-sm myBtn-midlight",
            buttonup_class: "myBtn myBtn-border myBtn-rounded myBtn-sm myBtn-midlight"
          });
				});
			}
		}

  };




  //!!!!!!!!!!!!!!!!!!!!
  // check mobile device
  //!!!!!!!!!!!!!!!!!!!!

  MINOVATE.isMobile = {
    Android: function() {
      return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function() {
      return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function() {
      return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function() {
      return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function() {
      return navigator.userAgent.match(/IEMobile/i);
    },
    any: function() {
      return (MINOVATE.isMobile.Android() || MINOVATE.isMobile.BlackBerry() || MINOVATE.isMobile.iOS() || MINOVATE.isMobile.Opera() || MINOVATE.isMobile.Windows());
    }
  };



  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // initialize after resize
  //!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.documentOnResize = {

		init: function(){

      var headerWrapOffset = $headerWrap.offset().top;

      var t = setTimeout( function(){
				MINOVATE.header.megaMenuWidth();
        MINOVATE.header.chooseMenuStyle(headerWrapOffset);
        MINOVATE.header.chooseLogo();
        MINOVATE.extra.html5Video();
			}, 500 );

		}

	};






  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // initialize when document ready
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.documentOnReady = {

		init: function(){
      MINOVATE.global.init();
			MINOVATE.header.init();
      MINOVATE.documentOnReady.windowscroll();
      MINOVATE.extra.init();
		},

    // run on window scrolling

    windowscroll: function(){

			var headerOffset = $header.offset().top;
			var headerWrapOffset = $headerWrap.offset().top;

			$window.on( 'scroll', function(){

				MINOVATE.global.goToTopScroll();
        MINOVATE.header.stickyHeader(headerWrapOffset);
				MINOVATE.header.chooseLogo();
        MINOVATE.slider.sliderParallax();

			});
		}

	};







  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // initialize when document load
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	MINOVATE.documentOnLoad = {

		init: function(){

		}

	};






  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // global variables
  //!!!!!!!!!!!!!!!!!!!!!!!!!

  var $window = $(window),
      $body = $('body'),
      $searchBoxWrapper = $('.search-box-wrapper'),
      $wrapper = $('#wrapper'),
      $header = $('#header'),
      $headerWrap = $('#header-wrap'),
      oldHeaderClasses = $header.attr('class'),
      defaultLogo = $('#branding').find('.brand-normal'),
      defaultLogoWidth = defaultLogo.find('img').outerWidth(),
      retinaLogo = $('#branding').find('.brand-retina'),
      defaultLogoImg = defaultLogo.find('img').attr('src'),
      retinaLogoImg = retinaLogo.find('img').attr('src'),
      defaultLightLogo = defaultLogo.attr('data-light-logo'),
      retinaLightLogo = retinaLogo.attr('data-light-logo'),
      $pageTitle = $('#page-title'),
      $slider = $('#slider'),
		  $sliderParallaxEl = $('.slider-parallax'),
      $menuToggler = $('#main-navbar-toggle'),
      $owlCarouselEl = $('.owl-carousel'),
      $goToTopEl = $('#gotoTop'),
      $mixitupEl = $('.mix-grid'),
      $shopCart = $('#shopping-cart'),
      $sidebarMenu = $('#sidebar-menu'),
      $bsSelectEl = $('.selectpicker'),
      $rangeSliderEl = $('.slider'),
      $touchSpinEl = $('.touchspin');






  //!!!!!!!!!!!!!
  // initializing
  //!!!!!!!!!!!!!
  $(document).ready( MINOVATE.documentOnReady.init );
  $window.load( MINOVATE.documentOnLoad.init );
  $window.on( 'resize', MINOVATE.documentOnResize.init );

});
