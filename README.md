# BlazorWithRedux

## Problemi Past/Future handler of UndoableObjects:

### Problema 1: Gestione Corretta della Cronologia degli Undo

**Descrizione**: Quando si effettua un nuovo cambiamento di stato dopo aver eseguito delle operazioni di undo, i cambiamenti non vengono correttamente aggiunti alla cronologia dei passati, causando un reset del flusso degli undo.

**Cause**: Questo è il comportamento attuale della libreria, ma è necessario modificare l'implementazione.

**Soluzione Proposta:** 
- Potrebbe essere logico supporre che quando si torna indietro di un certo numero di azioni, si sia liberi di andare avanti, ma dal momento in cui si effettua un nuovo cambiamento, il futuro viene cancellato.
	- In questo modo si può capire che il flusso degli undo ha ripreso da un punto "seguibile".
		- Questo è importante perché, ad esempio, se il futuro è 14 e si è arrivati a 13, se si effettua un'azione che porta allo stesso stato, si avrebbe uno stato duplicato e potrebbe essere poco intuitivo tornare al futuro (poiché sarebbe lo stesso stato).
		- Questo potrebbe essere un problema quando si desidera tornare indietro a uno stato futuro come 50, andando indietro di 7/8 stati, ma avendo aggiornato lo stato, non si ha più accesso al futuro e si può solo tornare indietro e fare azioni di undo al posto di redo.

**Implementazione:** Il codice necessario per affrontare questo problema è già presente nel progetto. Bisogna fare un'attenta revisione e apportare le modifiche necessarie per garantire il corretto funzionamento della cronologia degli undo.