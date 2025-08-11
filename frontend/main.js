const functionApi = 'https://getresumecounterr.azurewebsites.net/api/GetResumeCounter';

async function updateCounter() {
  const el = document.getElementById('counter');
  if (!el) return;

  try {
    const r = await fetch(functionApi, { cache: 'no-store' });
    if (!r.ok) throw new Error(`${r.status} ${r.statusText}`);
    el.textContent = (await r.text()).trim() || '0';
  } catch (e) {
    console.error('Counter fetch failed:', e);
    el.textContent = '—';
  }
}

updateCounter();
