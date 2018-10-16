import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxQRCodeModule } from 'ngx-qrcode2';
import { NgxSpinnerModule } from 'ngx-spinner';
// import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
// import { InMemoryDataService } from './in-memory-data.service';
import { ToastrModule } from 'ngx-toastr';

import { AboutComponent } from './about/about.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigurationService } from './core/configuration/configuration.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FooterComponent } from './layout/footer/footer.component';
import { MessagesComponent } from './core/messages/messages.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ResultsComponent } from './results/results.component';
import { SignalRService } from './core/signalr.service';
import { ViewPollDetailsComponent } from './view-poll-details/view-poll-details.component';


@NgModule({
  declarations: [
    AboutComponent,
    AppComponent,
    DashboardComponent,
    FooterComponent,
    MessagesComponent,
    NavbarComponent,
    PageNotFoundComponent,
    ResultsComponent,
    ViewPollDetailsComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    NgxSpinnerModule,
    NgxQRCodeModule,
    // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
    // and returns simulated server responses.
    // Remove it when a real server is ready to receive requests.
    // HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, {
    // dataEncapsulation: false
    // }),
  ],

  providers: [ConfigurationService,
    {
      // Here we request that configuration loading be done at app-
      // initialization time (prior to rendering)
      provide: APP_INITIALIZER,
      useFactory: (configService: ConfigurationService) =>
        () => configService.loadConfigurationData(),
      deps: [ConfigurationService],
      multi: true
    },
    SignalRService],
  bootstrap: [AppComponent],
})
export class AppModule {}
