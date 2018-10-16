import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxQRCodeModule } from 'ngx-qrcode2';

// import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
// import { InMemoryDataService } from './in-memory-data.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { VoteComponent } from './vote/vote.component';
import { ConfigurationService } from './core/configuration/configuration.service';
import { MessagesComponent } from './messages/messages.component';
import { FooterComponent } from './layout/footer/footer.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { ResultsComponent } from './results/results.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { SignalRService } from './signalr.service';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';


@NgModule({
  declarations: [
    AppComponent,
    VoteComponent,
    MessagesComponent,
    FooterComponent,
    NavbarComponent,
    ResultsComponent,
    PageNotFoundComponent,
    HomeComponent,
    AboutComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    NgxSpinnerModule,
    NgxQRCodeModule
    // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
    // and returns simulated server responses.
    // Remove it when a real server is ready to receive requests.
    // HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, {
    // dataEncapsulation: false
    // }),
  ],

  providers: [
    ConfigurationService,
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
  bootstrap: [AppComponent]
})
export class AppModule {}
