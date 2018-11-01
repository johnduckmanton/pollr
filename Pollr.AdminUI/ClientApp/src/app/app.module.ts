/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxQRCodeModule } from 'ngx-qrcode2';
import { NgxSpinnerModule } from 'ngx-spinner';
// import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
// import { InMemoryDataService } from './in-memory-data.service';
import { ToastrModule } from 'ngx-toastr';

import { AboutComponent } from './pages/about/about.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigurationService } from './core/configuration/configuration.service';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FooterComponent } from './layout/footer/footer.component';
import { LoadingService } from './core/loading/loading.service';
import { LoadingInterceptor } from './core/loading/loading.interceptor';
import { MessagesComponent } from './core/messages/messages.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { ResultChartComponent } from './pages/result-chart/result-chart.component';
import { ResultsComponent } from './pages/results/results.component';
import { SignalRService } from './core/signalr.service';
import { ViewPollDetailsComponent } from './pages/view-poll-details/view-poll-details.component';
import { VoteStatusComponent } from './pages/vote-status/vote-status.component';
import { ResultComponent } from './pages/result/result.component';


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
    VoteStatusComponent,
    ResultComponent,
    ResultChartComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    NgxSpinnerModule,
    NgxQRCodeModule,
    // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
    // and returns simulated server responses.
    // Remove it when a real server is ready to receive requests.
    // HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, {
    // dataEncapsulation: false
    // }),
    ReactiveFormsModule,
    ToastrModule.forRoot(),
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
    LoadingService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true
    },
    SignalRService],
  bootstrap: [AppComponent]
})
export class AppModule { }

export * from './core/loading/loading.service';
