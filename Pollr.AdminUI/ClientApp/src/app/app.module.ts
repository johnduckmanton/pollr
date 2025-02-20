/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MsalModule } from "@azure/msal-angular";
import { MsalInterceptor } from "@azure/msal-angular";
import { LogLevel } from "msal";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgHttpLoaderModule } from 'ng-http-loader';
import { NgxQRCodeModule } from 'ngx-qrcode2';
// import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
// import { InMemoryDataService } from './in-memory-data.service';
import { ToastrModule } from 'ngx-toastr';

import { AboutComponent } from './pages/about/about.component';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ConfigurationService } from './core/configuration/configuration.service';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FooterComponent } from './layout/footer/footer.component';
import { MessagesComponent } from './core/messages/messages.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { ResultChartComponent } from './pages/result-chart/result-chart.component';
import { ResultComponent } from './pages/result/result.component';
import { ResultsComponent } from './pages/results/results.component';
import { SignalRService } from './core/signalr.service';
import { ViewPollDetailsComponent } from './pages/view-poll-details/view-poll-details.component';
import { VoteStatusComponent } from './pages/vote-status/vote-status.component';

export function loggerCallback(logLevel, message, piiEnabled) {
  console.log("client logging" + message);
}

@NgModule({
  declarations: [
    AboutComponent,
    AppComponent,
    DashboardComponent,
    FooterComponent,
    MessagesComponent,
    NavbarComponent,
    PageNotFoundComponent,
    ResultChartComponent,
    ResultComponent,
    ResultsComponent,
    ViewPollDetailsComponent,
    VoteStatusComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MsalModule.forRoot({
      clientID: 'ccdbd728-bc9c-4c56-88f8-52a2c3baeccf',
      authority: "https://login.microsoftonline.com/8406ce5f-3181-49e4-be30-e180bd3765e9",
      validateAuthority: true,
      redirectUri: "http://localhost:82/",
      cacheLocation: "localStorage",
      postLogoutRedirectUri: "http://localhost:82/",
      navigateToLoginRequestUrl: true,
      popUp: false,
      consentScopes: ["user.read"],
      unprotectedResources: ["http://localhost:82/ClientConfiguration"],
      logger: loggerCallback,
      correlationId: '1234',
      level: LogLevel.Info,
      piiLoggingEnabled: true
    }
    ),
    NgbModule,
    NgHttpLoaderModule,
    NgxQRCodeModule,
    // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
    // and returns simulated server responses.
    // Remove it when a real server is ready to receive requests.
    // HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, {
    // dataEncapsulation: false
    // }),
    ReactiveFormsModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      closeButton: true
    }),
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
    //{ provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true },
    SignalRService],
  bootstrap: [AppComponent]
})
export class AppModule { }
