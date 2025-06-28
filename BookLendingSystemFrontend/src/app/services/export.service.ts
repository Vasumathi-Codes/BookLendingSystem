import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Injectable({
  providedIn: 'root'
})
export class ExportService {

  downloadCSV(data: any[], filename: string) {
    if (!data || !data.length) {
      return;
    }

    const separator = ',';
    const keys = Object.keys(data[0]);
    const csvContent =
      keys.join(separator) +
      '\n' +
      data.map(row => {
        return keys.map(k => JSON.stringify(row[k], (key, value) => (value === null ? '' : value))).join(separator);
      }).join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    if ((navigator as any).msSaveBlob) {
        (navigator as any).msSaveBlob(blob, filename);
    } else {
        // fallback for other browsers
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        URL.revokeObjectURL(link.href);
    }

  }

  downloadPDF(data: any[], columns: string[], filename: string) {
    const doc = new jsPDF();

    const headers = [columns];
    const rows = data.map(row => columns.map(col => row[col] || ''));

    autoTable(doc, {
      head: headers,
      body: rows,
    });

    doc.save(filename);
  }
}
